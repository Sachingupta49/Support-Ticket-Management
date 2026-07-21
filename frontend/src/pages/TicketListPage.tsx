import { useEffect, useState } from 'react';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import { getTickets } from '../api/tickets';
import { ApiClientError } from '../api/client';
import type { Ticket, TicketStatus } from '../types';
import { ErrorAlert } from '../components/ErrorAlert';
import { LoadingSpinner } from '../components/LoadingSpinner';
import { TicketTable } from '../components/TicketTable';
import { useDebounce } from '../hooks/useDebounce';
import { STATUS_LABELS } from '../types';

const statusOptions: { value: string; label: string }[] = [
  { value: '', label: 'All Statuses' },
  ...(['Open', 'InProgress', 'Resolved', 'Closed', 'Cancelled'] as TicketStatus[]).map((s) => ({
    value: s,
    label: STATUS_LABELS[s],
  })),
];

export function TicketListPage() {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [search, setSearch] = useState(searchParams.get('search') ?? '');
  const [status, setStatus] = useState(searchParams.get('status') ?? '');
  const debouncedSearch = useDebounce(search, 300);
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const params = new URLSearchParams();
    if (debouncedSearch) params.set('search', debouncedSearch);
    if (status) params.set('status', status);
    setSearchParams(params, { replace: true });
  }, [debouncedSearch, status, setSearchParams]);

  useEffect(() => {
    setLoading(true);
    getTickets(debouncedSearch || undefined, status || undefined)
      .then(setTickets)
      .catch((err) => {
        setError(err instanceof ApiClientError ? err.message : 'Unable to load tickets.');
      })
      .finally(() => setLoading(false));
  }, [debouncedSearch, status]);

  return (
    <div className="page">
      <div className="page-header">
        <h1>Tickets</h1>
        <Link to="/tickets/new" className="btn btn-primary">
          Create Ticket
        </Link>
      </div>

      <div className="filters-bar">
        <input
          type="search"
          placeholder="Search tickets..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="search-input"
        />
        <select value={status} onChange={(e) => setStatus(e.target.value)}>
          {statusOptions.map((opt) => (
            <option key={opt.value} value={opt.value}>
              {opt.label}
            </option>
          ))}
        </select>
      </div>

      {error && <ErrorAlert message={error} onDismiss={() => setError('')} />}
      {loading ? (
        <LoadingSpinner />
      ) : (
        <TicketTable
          tickets={tickets}
          onRowClick={(ticket) => navigate(`/tickets/${ticket.id}`)}
        />
      )}
    </div>
  );
}

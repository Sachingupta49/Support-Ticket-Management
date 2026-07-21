import { useEffect, useState } from 'react';
import { useLocation, useNavigate, useSearchParams } from 'react-router-dom';
import { getTickets } from '../api/tickets';
import { ApiClientError } from '../api/client';
import type { Ticket, TicketStatus } from '../types';
import { ErrorAlert } from '../components/ErrorAlert';
import { FlashMessage } from '../components/FlashMessage';
import { LoadingSpinner } from '../components/LoadingSpinner';
import { PageHeader } from '../components/PageHeader';
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
  const location = useLocation();
  const [searchParams, setSearchParams] = useSearchParams();
  const [flashMessage, setFlashMessage] = useState(
    (location.state as { message?: string } | null)?.message ?? '',
  );
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
    setError('');
    getTickets(debouncedSearch || undefined, status || undefined)
      .then(setTickets)
      .catch((err) => {
        setError(err instanceof ApiClientError ? err.message : 'Unable to load tickets.');
      })
      .finally(() => setLoading(false));
  }, [debouncedSearch, status]);

  return (
    <div className="page">
      <PageHeader
        title="Tickets"
        subtitle="Search, filter, and manage all support requests"
      />

      <div className="filters-bar panel">
        <div className="search-wrapper">
          <span className="search-icon" aria-hidden="true">⌕</span>
          <input
            type="search"
            placeholder="Search by title or description..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="search-input"
          />
        </div>
        <select value={status} onChange={(e) => setStatus(e.target.value)} className="filter-select">
          {statusOptions.map((opt) => (
            <option key={opt.value} value={opt.value}>
              {opt.label}
            </option>
          ))}
        </select>
      </div>

      {flashMessage && (
        <FlashMessage message={flashMessage} variant="error" onDismiss={() => setFlashMessage('')} />
      )}
      {error && <ErrorAlert message={error} onDismiss={() => setError('')} />}

      <section className="panel">
        {loading ? (
          <LoadingSpinner />
        ) : (
          <TicketTable
            tickets={tickets}
            onRowClick={(ticket) => navigate(`/tickets/${ticket.id}`)}
          />
        )}
      </section>
    </div>
  );
}

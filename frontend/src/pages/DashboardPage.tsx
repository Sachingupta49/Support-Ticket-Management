import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { getDashboardSummary } from '../api/tickets';
import { ApiClientError } from '../api/client';
import type { DashboardSummary } from '../types';
import { ErrorAlert } from '../components/ErrorAlert';
import { LoadingSpinner } from '../components/LoadingSpinner';
import { TicketTable } from '../components/TicketTable';
import { STATUS_LABELS } from '../types';
import type { TicketStatus } from '../types';

const statusOrder: TicketStatus[] = ['Open', 'InProgress', 'Resolved', 'Closed', 'Cancelled'];

const statusCardClass: Record<TicketStatus, string> = {
  Open: 'card-open',
  InProgress: 'card-inprogress',
  Resolved: 'card-resolved',
  Closed: 'card-closed',
  Cancelled: 'card-cancelled',
};

export function DashboardPage() {
  const navigate = useNavigate();
  const [summary, setSummary] = useState<DashboardSummary | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    getDashboardSummary()
      .then(setSummary)
      .catch((err) => {
        setError(err instanceof ApiClientError ? err.message : 'Unable to load dashboard.');
      })
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <LoadingSpinner />;
  if (error) return <ErrorAlert message={error} />;
  if (!summary) return null;

  return (
    <div className="page">
      <div className="page-header">
        <h1>Dashboard</h1>
        <Link to="/tickets/new" className="btn btn-primary">
          Create Ticket
        </Link>
      </div>

      <div className="summary-cards">
        <div className="summary-card card-total">
          <span className="card-label">Total Tickets</span>
          <span className="card-value">{summary.totalTickets}</span>
        </div>
        {statusOrder.map((status) => (
          <button
            key={status}
            type="button"
            className={`summary-card ${statusCardClass[status]}`}
            onClick={() => navigate(`/tickets?status=${status}`)}
          >
            <span className="card-label">{STATUS_LABELS[status]}</span>
            <span className="card-value">{summary.byStatus[status] ?? 0}</span>
          </button>
        ))}
      </div>

      <section className="section">
        <h2>Recent Tickets</h2>
        <TicketTable
          tickets={summary.recentTickets}
          onRowClick={(ticket) => navigate(`/tickets/${ticket.id}`)}
        />
      </section>
    </div>
  );
}

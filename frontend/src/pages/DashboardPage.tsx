import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getDashboardSummary } from '../api/tickets';
import { ApiClientError } from '../api/client';
import type { DashboardSummary } from '../types';
import { ErrorAlert } from '../components/ErrorAlert';
import { LoadingSpinner } from '../components/LoadingSpinner';
import { PageHeader } from '../components/PageHeader';
import { TicketTable } from '../components/TicketTable';
import { STATUS_LABELS } from '../types';
import type { TicketStatus } from '../types';

const statusOrder: TicketStatus[] = ['Open', 'InProgress', 'Resolved', 'Closed', 'Cancelled'];

const statusIcons: Record<TicketStatus, string> = {
  Open: '○',
  InProgress: '◐',
  Resolved: '✓',
  Closed: '■',
  Cancelled: '×',
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
      <PageHeader
        title="Dashboard"
        subtitle="Overview of your support ticket workload"
      />

      <div className="summary-cards">
        <div className="summary-card card-total">
          <span className="card-icon" aria-hidden="true">#</span>
          <span className="card-label">Total Tickets</span>
          <span className="card-value">{summary.totalTickets}</span>
        </div>
        {statusOrder.map((status) => (
          <button
            key={status}
            type="button"
            className={`summary-card card-${status.toLowerCase()}`}
            onClick={() => navigate(`/tickets?status=${status}`)}
          >
            <span className="card-icon" aria-hidden="true">{statusIcons[status]}</span>
            <span className="card-label">{STATUS_LABELS[status]}</span>
            <span className="card-value">{summary.byStatus[status] ?? 0}</span>
          </button>
        ))}
      </div>

      <section className="panel">
        <div className="panel-header">
          <h2>Recent Tickets</h2>
          <span className="panel-hint">Last 5 updated</span>
        </div>
        <TicketTable
          tickets={summary.recentTickets}
          onRowClick={(ticket) => navigate(`/tickets/${ticket.id}`)}
        />
      </section>
    </div>
  );
}

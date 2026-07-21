import type { TicketStatus } from '../types';
import { ALLOWED_TRANSITIONS, STATUS_LABELS } from '../types';

interface StatusSelectorProps {
  currentStatus: TicketStatus;
  onChange: (status: TicketStatus) => Promise<void>;
  disabled?: boolean;
}

export function StatusSelector({ currentStatus, onChange, disabled }: StatusSelectorProps) {
  const allowed = ALLOWED_TRANSITIONS[currentStatus];

  if (allowed.length === 0) {
    return <p className="status-hint">No further status changes allowed.</p>;
  }

  return (
    <div className="status-selector">
      <label htmlFor="status-change">Change Status</label>
      <div className="status-actions">
        {allowed.map((status) => (
          <button
            key={status}
            type="button"
            className="btn btn-outline"
            disabled={disabled}
            onClick={() => onChange(status)}
          >
            → {STATUS_LABELS[status]}
          </button>
        ))}
      </div>
    </div>
  );
}

import type { TicketStatus } from '../types';
import { ALLOWED_TRANSITIONS, STATUS_LABELS } from '../types';

interface StatusSelectorProps {
  currentStatus: TicketStatus;
  onChange: (status: TicketStatus) => Promise<void>;
  disabled?: boolean;
}

export function StatusSelector({ currentStatus, onChange, disabled }: StatusSelectorProps) {
  const allowed = ALLOWED_TRANSITIONS[currentStatus];

  return (
    <div className="status-selector">
      <p className="status-current">
        Current: <strong>{STATUS_LABELS[currentStatus]}</strong>
      </p>
      {allowed.length === 0 ? (
        <p className="status-hint">This ticket has reached a terminal state.</p>
      ) : (
        <div className="status-actions">
          {allowed.map((status) => (
            <button
              key={status}
              type="button"
              className={`btn btn-status btn-status-${status.toLowerCase()}`}
              disabled={disabled}
              onClick={() => onChange(status)}
            >
              Move to {STATUS_LABELS[status]}
            </button>
          ))}
        </div>
      )}
    </div>
  );
}

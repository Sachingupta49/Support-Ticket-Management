import type { TicketPriority, TicketStatus } from '../types';
import { STATUS_LABELS } from '../types';

const statusClass: Record<TicketStatus, string> = {
  Open: 'badge-open',
  InProgress: 'badge-inprogress',
  Resolved: 'badge-resolved',
  Closed: 'badge-closed',
  Cancelled: 'badge-cancelled',
};

const priorityClass: Record<TicketPriority, string> = {
  Low: 'badge-priority-low',
  Medium: 'badge-priority-medium',
  High: 'badge-priority-high',
  Critical: 'badge-priority-critical',
};

interface StatusBadgeProps {
  status: TicketStatus;
}

export function StatusBadge({ status }: StatusBadgeProps) {
  return (
    <span className={`badge ${statusClass[status]}`}>
      {STATUS_LABELS[status]}
    </span>
  );
}

interface PriorityBadgeProps {
  priority: TicketPriority;
}

export function PriorityBadge({ priority }: PriorityBadgeProps) {
  return <span className={`badge ${priorityClass[priority]}`}>{priority}</span>;
}

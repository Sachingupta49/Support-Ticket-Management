import { Link } from 'react-router-dom';
import type { Ticket } from '../types';
import { PriorityBadge, StatusBadge } from './Badges';

interface TicketTableProps {
  tickets: Ticket[];
  onRowClick?: (ticket: Ticket) => void;
}

export function TicketTable({ tickets, onRowClick }: TicketTableProps) {
  if (tickets.length === 0) {
    return <p className="empty-state">No tickets found.</p>;
  }

  return (
    <div className="table-wrapper">
      <table className="data-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Title</th>
            <th>Status</th>
            <th>Priority</th>
            <th>Assignee</th>
            <th>Updated</th>
          </tr>
        </thead>
        <tbody>
          {tickets.map((ticket) => (
            <tr
              key={ticket.id}
              className={onRowClick ? 'clickable-row' : undefined}
              onClick={() => onRowClick?.(ticket)}
            >
              <td>{ticket.id}</td>
              <td>
                {onRowClick ? (
                  <Link to={`/tickets/${ticket.id}`} onClick={(e) => e.stopPropagation()}>
                    {ticket.title}
                  </Link>
                ) : (
                  ticket.title
                )}
              </td>
              <td>
                <StatusBadge status={ticket.status} />
              </td>
              <td>
                <PriorityBadge priority={ticket.priority} />
              </td>
              <td>{ticket.assigneeName}</td>
              <td>{new Date(ticket.updatedAt).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

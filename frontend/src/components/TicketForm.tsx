import { useEffect, useState } from 'react';
import type { CreateTicketRequest, TicketPriority, UpdateTicketRequest, User } from '../types';

interface TicketFormProps {
  initialValues?: Partial<CreateTicketRequest>;
  users: User[];
  submitLabel: string;
  onSubmit: (data: CreateTicketRequest | UpdateTicketRequest) => Promise<void>;
  onCancel: () => void;
  fieldErrors?: Record<string, string[]>;
}

const priorities: TicketPriority[] = ['Low', 'Medium', 'High', 'Critical'];

export function TicketForm({
  initialValues,
  users,
  submitLabel,
  onSubmit,
  onCancel,
  fieldErrors,
}: TicketFormProps) {
  const [title, setTitle] = useState(initialValues?.title ?? '');
  const [description, setDescription] = useState(initialValues?.description ?? '');
  const [priority, setPriority] = useState<TicketPriority>(initialValues?.priority ?? 'Medium');
  const [assigneeId, setAssigneeId] = useState(initialValues?.assigneeId ?? users[0]?.id ?? 0);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    if (users.length > 0 && assigneeId === 0) {
      setAssigneeId(users[0].id);
    }
  }, [users, assigneeId]);

  const getFieldError = (field: string) => fieldErrors?.[field]?.[0];

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    try {
      await onSubmit({ title, description, priority, assigneeId });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form className="ticket-form" onSubmit={handleSubmit}>
      <div className="form-group">
        <label htmlFor="title">Title *</label>
        <input
          id="title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          maxLength={200}
          required
        />
        {getFieldError('Title') && <span className="field-error">{getFieldError('Title')}</span>}
      </div>

      <div className="form-group">
        <label htmlFor="description">Description *</label>
        <textarea
          id="description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          rows={5}
          required
        />
        {getFieldError('Description') && (
          <span className="field-error">{getFieldError('Description')}</span>
        )}
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="priority">Priority *</label>
          <select
            id="priority"
            value={priority}
            onChange={(e) => setPriority(e.target.value as TicketPriority)}
          >
            {priorities.map((p) => (
              <option key={p} value={p}>
                {p}
              </option>
            ))}
          </select>
          {getFieldError('Priority') && (
            <span className="field-error">{getFieldError('Priority')}</span>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="assignee">Assignee *</label>
          <select
            id="assignee"
            value={assigneeId}
            onChange={(e) => setAssigneeId(Number(e.target.value))}
          >
            {users.map((user) => (
              <option key={user.id} value={user.id}>
                {user.name}
              </option>
            ))}
          </select>
          {getFieldError('AssigneeId') && (
            <span className="field-error">{getFieldError('AssigneeId')}</span>
          )}
        </div>
      </div>

      <div className="form-actions">
        <button type="button" className="btn btn-secondary" onClick={onCancel}>
          Cancel
        </button>
        <button type="submit" className="btn btn-primary" disabled={submitting}>
          {submitting ? 'Saving...' : submitLabel}
        </button>
      </div>
    </form>
  );
}

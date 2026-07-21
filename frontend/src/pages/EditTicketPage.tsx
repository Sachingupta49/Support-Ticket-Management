import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getTicket, getUsers, updateTicket } from '../api/tickets';
import { ApiClientError } from '../api/client';
import type { User } from '../types';
import { ErrorAlert } from '../components/ErrorAlert';
import { LoadingSpinner } from '../components/LoadingSpinner';
import { TicketForm } from '../components/TicketForm';

export function EditTicketPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const ticketId = Number(id);

  const [users, setUsers] = useState<User[]>([]);
  const [initialValues, setInitialValues] = useState<{
    title: string;
    description: string;
    priority: 'Low' | 'Medium' | 'High' | 'Critical';
    assigneeId: number;
  } | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [fieldErrors, setFieldErrors] = useState<Record<string, string[]>>();

  useEffect(() => {
    Promise.all([getTicket(ticketId), getUsers()])
      .then(([ticket, usersData]) => {
        setInitialValues({
          title: ticket.title,
          description: ticket.description,
          priority: ticket.priority,
          assigneeId: ticket.assigneeId,
        });
        setUsers(usersData);
      })
      .catch((err) => {
        if (err instanceof ApiClientError && err.status === 404) {
          navigate('/tickets', { state: { message: 'Ticket not found.' } });
        } else {
          setError(err instanceof ApiClientError ? err.message : 'Unable to load ticket.');
        }
      })
      .finally(() => setLoading(false));
  }, [ticketId, navigate]);

  const handleSubmit = async (data: Parameters<typeof updateTicket>[1]) => {
    setError('');
    setFieldErrors(undefined);
    try {
      await updateTicket(ticketId, data);
      navigate(`/tickets/${ticketId}`);
    } catch (err) {
      if (err instanceof ApiClientError) {
        setError(err.message);
        setFieldErrors(err.errors);
      } else {
        setError('Unable to update ticket. Please try again.');
      }
      throw err;
    }
  };

  if (loading) return <LoadingSpinner />;
  if (error && !initialValues) return <ErrorAlert message={error} />;
  if (!initialValues) return null;

  return (
    <div className="page">
      <div className="page-header">
        <h1>Edit Ticket</h1>
      </div>
      {error && <ErrorAlert message={error} fieldErrors={fieldErrors} onDismiss={() => setError('')} />}
      <TicketForm
        initialValues={initialValues}
        users={users}
        submitLabel="Save Changes"
        onSubmit={handleSubmit}
        onCancel={() => navigate(`/tickets/${ticketId}`)}
        fieldErrors={fieldErrors}
      />
    </div>
  );
}

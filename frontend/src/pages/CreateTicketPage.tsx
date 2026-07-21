import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createTicket, getUsers } from '../api/tickets';
import { ApiClientError } from '../api/client';
import type { User } from '../types';
import { ErrorAlert } from '../components/ErrorAlert';
import { LoadingSpinner } from '../components/LoadingSpinner';
import { TicketForm } from '../components/TicketForm';

export function CreateTicketPage() {
  const navigate = useNavigate();
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [fieldErrors, setFieldErrors] = useState<Record<string, string[]>>();

  useEffect(() => {
    getUsers()
      .then(setUsers)
      .catch((err) => {
        setError(err instanceof ApiClientError ? err.message : 'Unable to load users.');
      })
      .finally(() => setLoading(false));
  }, []);

  const handleSubmit = async (data: Parameters<typeof createTicket>[0]) => {
    setError('');
    setFieldErrors(undefined);
    try {
      const ticket = await createTicket(data);
      navigate(`/tickets/${ticket.id}`);
    } catch (err) {
      if (err instanceof ApiClientError) {
        setError(err.message);
        setFieldErrors(err.errors);
      } else {
        setError('Unable to create ticket. Please try again.');
      }
      throw err;
    }
  };

  if (loading) return <LoadingSpinner />;

  return (
    <div className="page">
      <div className="page-header">
        <h1>Create Ticket</h1>
      </div>
      {error && <ErrorAlert message={error} fieldErrors={fieldErrors} onDismiss={() => setError('')} />}
      <TicketForm
        users={users}
        submitLabel="Create Ticket"
        onSubmit={handleSubmit}
        onCancel={() => navigate('/tickets')}
        fieldErrors={fieldErrors}
      />
    </div>
  );
}

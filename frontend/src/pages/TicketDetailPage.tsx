import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import {
  changeTicketStatus,
  createComment,
  getComments,
  getTicket,
  getUsers,
} from '../api/tickets';
import { ApiClientError } from '../api/client';
import type { Comment, Ticket, TicketStatus, User } from '../types';
import { PriorityBadge, StatusBadge } from '../components/Badges';
import { CommentForm, CommentList } from '../components/Comments';
import { ErrorAlert } from '../components/ErrorAlert';
import { LoadingSpinner } from '../components/LoadingSpinner';
import { StatusSelector } from '../components/StatusSelector';

export function TicketDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const ticketId = Number(id);

  const [ticket, setTicket] = useState<Ticket | null>(null);
  const [comments, setComments] = useState<Comment[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [statusError, setStatusError] = useState('');
  const [commentFieldErrors, setCommentFieldErrors] = useState<Record<string, string[]>>();
  const [statusChanging, setStatusChanging] = useState(false);

  const loadData = async () => {
    try {
      const [ticketData, commentsData, usersData] = await Promise.all([
        getTicket(ticketId),
        getComments(ticketId),
        getUsers(),
      ]);
      setTicket(ticketData);
      setComments(commentsData);
      setUsers(usersData);
    } catch (err) {
      if (err instanceof ApiClientError && err.status === 404) {
        navigate('/tickets', { state: { message: 'Ticket not found.' } });
      } else {
        setError(err instanceof ApiClientError ? err.message : 'Unable to load ticket.');
      }
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (!ticketId) return;
    loadData();
  }, [ticketId]);

  const handleStatusChange = async (status: TicketStatus) => {
    setStatusError('');
    setStatusChanging(true);
    try {
      const updated = await changeTicketStatus(ticketId, status);
      setTicket(updated);
    } catch (err) {
      setStatusError(err instanceof ApiClientError ? err.message : 'Unable to change status.');
    } finally {
      setStatusChanging(false);
    }
  };

  const handleAddComment = async (authorId: number, body: string) => {
    setCommentFieldErrors(undefined);
    try {
      const comment = await createComment(ticketId, { authorId, body });
      setComments((prev) => [...prev, comment]);
    } catch (err) {
      if (err instanceof ApiClientError) {
        setCommentFieldErrors(err.errors);
        throw err;
      }
      throw err;
    }
  };

  if (loading) return <LoadingSpinner />;
  if (error) return <ErrorAlert message={error} />;
  if (!ticket) return null;

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <Link to="/tickets" className="back-link">
            ← Back to Tickets
          </Link>
          <h1>{ticket.title}</h1>
          <div className="ticket-meta">
            <StatusBadge status={ticket.status} />
            <PriorityBadge priority={ticket.priority} />
            <span>Assignee: {ticket.assigneeName}</span>
          </div>
        </div>
        <Link to={`/tickets/${ticket.id}/edit`} className="btn btn-secondary">
          Edit
        </Link>
      </div>

      <section className="section card">
        <h2>Details</h2>
        <p className="ticket-description">{ticket.description}</p>
        <div className="ticket-dates">
          <span>Created: {new Date(ticket.createdAt).toLocaleString()}</span>
          <span>Updated: {new Date(ticket.updatedAt).toLocaleString()}</span>
        </div>
      </section>

      <section className="section card">
        <h2>Status</h2>
        {statusError && <ErrorAlert message={statusError} onDismiss={() => setStatusError('')} />}
        <StatusSelector
          currentStatus={ticket.status}
          onChange={handleStatusChange}
          disabled={statusChanging}
        />
      </section>

      <section className="section card">
        <h2>Comments</h2>
        <CommentList comments={comments} />
        <CommentForm
          users={users}
          defaultAuthorId={users[0]?.id}
          onSubmit={handleAddComment}
          fieldErrors={commentFieldErrors}
        />
      </section>
    </div>
  );
}

import { useState } from 'react';
import type { Comment, User } from '../types';

interface CommentListProps {
  comments: Comment[];
}

export function CommentList({ comments }: CommentListProps) {
  if (comments.length === 0) {
    return <p className="empty-state">No comments yet.</p>;
  }

  return (
    <div className="comment-list">
      {comments.map((comment) => (
        <div key={comment.id} className="comment-item">
          <div className="comment-meta">
            <strong>{comment.authorName}</strong>
            <span>{new Date(comment.createdAt).toLocaleString()}</span>
          </div>
          <p>{comment.body}</p>
        </div>
      ))}
    </div>
  );
}

interface CommentFormProps {
  users: User[];
  defaultAuthorId?: number;
  onSubmit: (authorId: number, body: string) => Promise<void>;
  fieldErrors?: Record<string, string[]>;
}

export function CommentForm({ users, defaultAuthorId, onSubmit, fieldErrors }: CommentFormProps) {
  const [authorId, setAuthorId] = useState(defaultAuthorId ?? users[0]?.id ?? 0);
  const [body, setBody] = useState('');
  const [submitting, setSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    try {
      await onSubmit(authorId, body);
      setBody('');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form className="comment-form" onSubmit={handleSubmit}>
      <div className="form-row">
        <div className="form-group">
          <label htmlFor="author">Author</label>
          <select
            id="author"
            value={authorId}
            onChange={(e) => setAuthorId(Number(e.target.value))}
          >
            {users.map((user) => (
              <option key={user.id} value={user.id}>
                {user.name}
              </option>
            ))}
          </select>
        </div>
      </div>
      <div className="form-group">
        <label htmlFor="comment-body">Add Comment</label>
        <textarea
          id="comment-body"
          value={body}
          onChange={(e) => setBody(e.target.value)}
          rows={3}
          required
        />
        {fieldErrors?.Body?.[0] && <span className="field-error">{fieldErrors.Body[0]}</span>}
      </div>
      <button type="submit" className="btn btn-primary" disabled={submitting}>
        {submitting ? 'Posting...' : 'Post Comment'}
      </button>
    </form>
  );
}

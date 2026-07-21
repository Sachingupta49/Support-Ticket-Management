export type TicketStatus = 'Open' | 'InProgress' | 'Resolved' | 'Closed' | 'Cancelled';
export type TicketPriority = 'Low' | 'Medium' | 'High' | 'Critical';

export interface User {
  id: number;
  name: string;
  email: string;
}

export interface Ticket {
  id: number;
  title: string;
  description: string;
  status: TicketStatus;
  priority: TicketPriority;
  assigneeId: number;
  assigneeName: string;
  createdAt: string;
  updatedAt: string;
}

export interface Comment {
  id: number;
  ticketId: number;
  authorId: number;
  authorName: string;
  body: string;
  createdAt: string;
}

export interface DashboardSummary {
  totalTickets: number;
  byStatus: Record<string, number>;
  recentTickets: Ticket[];
}

export interface CreateTicketRequest {
  title: string;
  description: string;
  priority: TicketPriority;
  assigneeId: number;
}

export interface UpdateTicketRequest {
  title: string;
  description: string;
  priority: TicketPriority;
  assigneeId: number;
}

export interface CreateCommentRequest {
  authorId: number;
  body: string;
}

export interface ApiError {
  title?: string;
  detail?: string;
  errors?: Record<string, string[]>;
}

export const STATUS_LABELS: Record<TicketStatus, string> = {
  Open: 'Open',
  InProgress: 'In Progress',
  Resolved: 'Resolved',
  Closed: 'Closed',
  Cancelled: 'Cancelled',
};

export const ALLOWED_TRANSITIONS: Record<TicketStatus, TicketStatus[]> = {
  Open: ['InProgress', 'Cancelled'],
  InProgress: ['Resolved', 'Cancelled'],
  Resolved: ['Closed'],
  Closed: [],
  Cancelled: [],
};

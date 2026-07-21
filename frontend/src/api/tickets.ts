import { apiGet, apiPatch, apiPost, apiPut } from './client';
import type {
  Comment,
  CreateCommentRequest,
  CreateTicketRequest,
  DashboardSummary,
  Ticket,
  UpdateTicketRequest,
  User,
} from '../types';

export const getDashboardSummary = () => apiGet<DashboardSummary>('/dashboard/summary');

export const getUsers = () => apiGet<User[]>('/users');

export const getTickets = (search?: string, status?: string) => {
  const params = new URLSearchParams();
  if (search) params.set('search', search);
  if (status) params.set('status', status);
  const query = params.toString();
  return apiGet<Ticket[]>(`/tickets${query ? `?${query}` : ''}`);
};

export const getTicket = (id: number) => apiGet<Ticket>(`/tickets/${id}`);

export const createTicket = (data: CreateTicketRequest) =>
  apiPost<Ticket>('/tickets', data);

export const updateTicket = (id: number, data: UpdateTicketRequest) =>
  apiPut<Ticket>(`/tickets/${id}`, data);

export const changeTicketStatus = (id: number, status: string) =>
  apiPatch<Ticket>(`/tickets/${id}/status`, { status });

export const getComments = (ticketId: number) =>
  apiGet<Comment[]>(`/tickets/${ticketId}/comments`);

export const createComment = (ticketId: number, data: CreateCommentRequest) =>
  apiPost<Comment>(`/tickets/${ticketId}/comments`, data);

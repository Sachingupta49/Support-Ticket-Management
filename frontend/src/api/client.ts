import type { ApiError } from '../types';

const API_BASE = import.meta.env.VITE_API_BASE_URL ?? '/api';

export class ApiClientError extends Error {
  status: number;
  errors?: Record<string, string[]>;

  constructor(status: number, message: string, errors?: Record<string, string[]>) {
    super(message);
    this.status = status;
    this.errors = errors;
  }
}

async function request<T>(url: string, options?: RequestInit): Promise<T> {
  let response: Response;

  try {
    response = await fetch(url, options);
  } catch {
    throw new ApiClientError(0, 'Unable to connect. Please ensure the API is running and try again.');
  }

  if (response.ok) {
    if (response.status === 204) {
      return undefined as T;
    }
    return response.json() as Promise<T>;
  }

  let message = 'An unexpected error occurred.';
  let errors: Record<string, string[]> | undefined;

  try {
    const body = (await response.json()) as ApiError;
    message = body.detail ?? body.title ?? message;
    errors = body.errors;
  } catch {
    // ignore parse errors
  }

  throw new ApiClientError(response.status, message, errors);
}

export async function apiGet<T>(path: string): Promise<T> {
  return request<T>(`${API_BASE}${path}`);
}

export async function apiPost<T>(path: string, body: unknown): Promise<T> {
  return request<T>(`${API_BASE}${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
}

export async function apiPut<T>(path: string, body: unknown): Promise<T> {
  return request<T>(`${API_BASE}${path}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
}

export async function apiPatch<T>(path: string, body: unknown): Promise<T> {
  return request<T>(`${API_BASE}${path}`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
}

export async function checkApiHealth(): Promise<boolean> {
  try {
    await apiGet<{ status: string }>('/health');
    return true;
  } catch {
    return false;
  }
}

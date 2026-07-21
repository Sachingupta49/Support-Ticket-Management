import { Link, Outlet } from 'react-router-dom';
import { ApiStatusBanner } from './ApiStatusBanner';

export function Layout() {
  return (
    <div className="app-layout">
      <header className="app-header">
        <div className="header-content">
          <Link to="/" className="logo">
            Support Tickets
          </Link>
          <nav>
            <Link to="/">Dashboard</Link>
            <Link to="/tickets">Tickets</Link>
            <Link to="/tickets/new" className="btn btn-primary btn-sm">
              + New Ticket
            </Link>
          </nav>
        </div>
      </header>
      <ApiStatusBanner />
      <main className="app-main">
        <Outlet />
      </main>
    </div>
  );
}

import { Link, NavLink, Outlet } from 'react-router-dom';
import { ApiStatusBanner } from './ApiStatusBanner';

export function Layout() {
  return (
    <div className="app-layout">
      <header className="app-header">
        <div className="header-content">
          <Link to="/" className="logo">
            <span className="logo-icon" aria-hidden="true">◆</span>
            SupportDesk
          </Link>
          <nav className="main-nav">
            <NavLink to="/" end className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}>
              Dashboard
            </NavLink>
            <NavLink to="/tickets" className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}>
              Tickets
            </NavLink>
            <NavLink to="/tickets/new" className="btn btn-primary btn-sm nav-cta">
              + New Ticket
            </NavLink>
          </nav>
        </div>
      </header>
      <ApiStatusBanner />
      <main className="app-main">
        <Outlet />
      </main>
      <footer className="app-footer">
        <span>Support Ticket Management System</span>
        <span className="footer-dot">·</span>
        <span>Assessment Project</span>
      </footer>
    </div>
  );
}

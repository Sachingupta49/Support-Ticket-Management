import { useEffect, useState } from 'react';
import { checkApiHealth } from '../api/client';
import { ErrorAlert } from './ErrorAlert';

export function ApiStatusBanner() {
  const [apiOnline, setApiOnline] = useState<boolean | null>(null);

  useEffect(() => {
    checkApiHealth().then(setApiOnline);
  }, []);

  if (apiOnline !== false) {
    return null;
  }

  return (
    <div className="api-status-banner">
      <ErrorAlert message="API is not reachable. Start the backend with: dotnet run --project src/SupportTicket.API --launch-profile http" />
    </div>
  );
}

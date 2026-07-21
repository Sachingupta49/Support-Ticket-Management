interface ErrorAlertProps {
  message: string;
  fieldErrors?: Record<string, string[]>;
  onDismiss?: () => void;
}

export function ErrorAlert({ message, fieldErrors, onDismiss }: ErrorAlertProps) {
  return (
    <div className="alert alert-error" role="alert">
      <div className="alert-header">
        <strong>Error</strong>
        {onDismiss && (
          <button type="button" className="alert-dismiss" onClick={onDismiss}>
            ×
          </button>
        )}
      </div>
      <p>{message}</p>
      {fieldErrors && (
        <ul>
          {Object.entries(fieldErrors).map(([field, messages]) =>
            messages.map((msg) => (
              <li key={`${field}-${msg}`}>
                <strong>{field}:</strong> {msg}
              </li>
            )),
          )}
        </ul>
      )}
    </div>
  );
}

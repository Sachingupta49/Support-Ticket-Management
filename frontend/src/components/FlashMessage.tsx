interface FlashMessageProps {
  message: string;
  variant?: 'info' | 'success' | 'error';
  onDismiss: () => void;
}

export function FlashMessage({ message, variant = 'info', onDismiss }: FlashMessageProps) {
  return (
    <div className={`flash-message flash-${variant}`} role="status">
      <span>{message}</span>
      <button type="button" className="alert-dismiss" onClick={onDismiss} aria-label="Dismiss">
        ×
      </button>
    </div>
  );
}

export type AlertType = 'success' | 'danger' | 'warning';

export interface AppAlert {
  type: AlertType;
  message: string;
}
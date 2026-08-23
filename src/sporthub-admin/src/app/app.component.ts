import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { SpinnerComponent } from './shared/components/spinner/spinner.component';
import { AlertComponent } from './shared/components/alert/alert.component';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SpinnerComponent, AlertComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  protected readonly title = signal('sporthub-admin');
}

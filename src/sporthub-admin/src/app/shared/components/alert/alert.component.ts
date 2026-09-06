import { Component, OnInit, inject } from '@angular/core';
import { AlertService } from '../../service/alert.service';

@Component({
  selector: 'app-alert',
  standalone: true,
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.css'
})
export class AlertComponent implements OnInit {
  readonly alertService = inject(AlertService);
  constructor() { }

  ngOnInit() {
  }

}

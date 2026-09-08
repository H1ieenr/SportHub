import { Component, OnInit, inject } from '@angular/core';
import { AlertService } from '../../service/alert.service';
import { IconDirective, IconService } from '@ant-design/icons-angular';
import { CheckCircleFill, CloseCircleFill, ExclamationCircleFill } from '@ant-design/icons-angular/icons';

@Component({
  selector: 'app-alert',
  standalone: true,
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss',
  imports: [IconDirective]
})
export class AlertComponent implements OnInit {
  readonly alertService = inject(AlertService);
  private iconService = inject(IconService);

  constructor() {
    this.iconService.addIcon(...[CheckCircleFill, CloseCircleFill, ExclamationCircleFill]);
  }

  ngOnInit() {
  }

  getIconType(type: string): string {
    if (type === 'success') return 'check-circle';
    if (type === 'danger') return 'close-circle';
    return 'exclamation-circle'; 
  }
}

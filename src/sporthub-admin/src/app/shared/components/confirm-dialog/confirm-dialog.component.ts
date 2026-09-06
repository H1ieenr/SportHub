import { Component, inject } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { IconDirective, IconService } from '@ant-design/icons-angular';
import { CloseCircleOutline, DeleteFill } from '@ant-design/icons-angular/icons';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [IconDirective],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.scss'
})
export class ConfirmDialogComponent {
  private iconService = inject(IconService);
  readonly activeModal = inject(NgbActiveModal);

  title = 'Xác nhận';
  message = 'Bạn có chắc chắn muốn thực hiện thao tác này?';
  confirmText = 'Xoá';
  cancelText = 'Huỷ';

  constructor() {
    this.iconService.addIcon(...[CloseCircleOutline, DeleteFill]);
  }

  onConfirm() {
    this.activeModal.close(true);
  }

  onCancel() {
    this.activeModal.dismiss(false);
  }
}
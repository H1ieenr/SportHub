import { Component, inject } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.scss'
})
export class ConfirmDialogComponent {
  readonly activeModal = inject(NgbActiveModal);

  // đổi từ input() sang property thường vì component này
  // được mở qua NgbModal.open(), không qua template binding
  title = 'Xác nhận';
  message = 'Bạn có chắc chắn muốn thực hiện thao tác này?';
  confirmText = 'Xoá';
  cancelText = 'Huỷ';

  onConfirm() {
    this.activeModal.close(true);
  }

  onCancel() {
    this.activeModal.dismiss(false);
  }
}
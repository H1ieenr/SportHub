import { Injectable, inject } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmDialogComponent } from '../components/confirm-dialog/confirm-dialog.component';

export interface ConfirmDialogOptions {
    title?: string;
    message?: string;
    confirmText?: string;
    cancelText?: string;
}

@Injectable({ providedIn: 'root' })
export class ConfirmDialogService {
    private modalService = inject(NgbModal);

    /**
     * Mở dialog xác nhận, trả về true nếu người dùng bấm xác nhận,
     * false nếu huỷ hoặc đóng dialog.
     */
    confirm(options: ConfirmDialogOptions = {}): Promise<boolean> {
        const modalRef = this.modalService.open(ConfirmDialogComponent, {
            centered: true,
            size: 'sm'
        });

        const instance = modalRef.componentInstance as ConfirmDialogComponent;
        if (options.title) instance.title = options.title;
        if (options.message) instance.message = options.message;
        if (options.confirmText) instance.confirmText = options.confirmText;
        if (options.cancelText) instance.cancelText = options.cancelText;

        return modalRef.result.then(
            (result) => result === true,
            () => false
        );
    }
}
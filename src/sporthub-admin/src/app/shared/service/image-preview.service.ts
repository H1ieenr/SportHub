import { Injectable, inject } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ImagePreviewComponent } from '../components/image-preview/image-preview.component';

@Injectable({ providedIn: 'root' })
export class ImagePreviewService {
  private modalService = inject(NgbModal);

  open(imageUrl: string, title = '') {
    const modalRef = this.modalService.open(ImagePreviewComponent, {
      centered: true,
      size: 'lg',
      modalDialogClass: 'app-image-preview-dialog'
    });

    const instance = modalRef.componentInstance as ImagePreviewComponent;
    instance.imageUrl = imageUrl;
    instance.title = title;
  }
}
import { ChangeDetectionStrategy, Component, forwardRef, input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ClassicEditor, Bold, Essentials, Italic, Link, List, Paragraph, Underline } from 'ckeditor5';

@Component({
  selector: 'app-rich-text-editor',
  standalone: true,
  imports: [CKEditorModule],
  templateUrl: './rich-text-editor.component.html',
  styleUrl: './rich-text-editor.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RichTextEditorComponent),
      multi: true
    }
  ]
})
export class RichTextEditorComponent implements ControlValueAccessor {
  readonly placeholder = input<string>('Nhập nội dung...');

  readonly Editor = ClassicEditor;
  readonly config = {
    licenseKey: 'GPL',
    plugins: [Essentials, Bold, Italic, Underline, Link, List, Paragraph],
    toolbar: ['bold', 'italic', 'underline', '|', 'link', 'bulletedList', 'numberedList', '|', 'undo', 'redo']
  };

  value = '';
  disabled = false;

  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.value = value || '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onEditorChange({ editor }: { editor: ClassicEditor }): void {
    const data = editor.getData();
    this.value = data;
    this.onChange(data);
  }

  onBlur(): void {
    this.onTouched();
  }
}
import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Pipe({
  name: 'safeHtml'
})
export class SafeHtmlPipe implements PipeTransform {
  constructor(private _sanitized: DomSanitizer) {
  }

  /**
   * @inheritdoc
   * @param value
   */
  public transform(value: string): SafeHtml {
    return this._sanitized.bypassSecurityTrustHtml(value);
  }
}

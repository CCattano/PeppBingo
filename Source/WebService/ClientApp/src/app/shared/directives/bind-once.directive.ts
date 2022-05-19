import {Directive, EmbeddedViewRef, NgZone, TemplateRef, ViewContainerRef} from '@angular/core';

@Directive({
  selector: '[bindOnce]',
})
export class BindOnceDirective {
  constructor(template: TemplateRef<any>, container: ViewContainerRef, zone: NgZone) {
    zone.runOutsideAngular(() => {
      const view: EmbeddedViewRef<any> = container.createEmbeddedView(template);
      setTimeout(() => view.detach());
    });
  }
}

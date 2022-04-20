import { Component } from '@angular/core';
import { v4 as newUUID } from 'uuid';
import { IToastInfo, ToastService } from '../../service/toast.service';

interface IInternalToastInfo extends IToastInfo {
  /**
   * Unique value to represent the toast
   */
  toastId: string;
  /**
   * Classes to be applied to the toast being displayed
   */
  cssClassList?: string;
}

enum ToastTypeEnum {
  Success,
  Danger,
  Info
}

@Component({
  selector: 'app-toast-container',
  templateUrl: './toast-container.component.html',
  styles: [`
    :host {
      position: fixed;
      bottom: 0;
      right: 0;
      padding: 2rem;
      z-index: 2;
    }
  `]
})
export class ToastContainerComponent {
  public _toasts: IInternalToastInfo[] = [];

  constructor(_toastService: ToastService) {
    _toastService.init(this);
  }

  /**
   * Opens a success toast displaying the information provided
   * @param toast
   */
  public openSuccessToast(toast: IToastInfo): void {
    const successToast: IInternalToastInfo = {
      ...toast,
      toastId: newUUID(),
      cssClassList: this._getToastStyle(ToastTypeEnum.Success)
    };
    this._toasts.push(successToast);
  }

  /**
   * Opens a danger toast displaying the information provided
   * @param toast
   */
  public openDangerToast(toast: IToastInfo): void {
    const successToast: IInternalToastInfo = {
      ...toast,
      toastId: newUUID(),
      cssClassList: this._getToastStyle(ToastTypeEnum.Danger)
    };
    this._toasts.push(successToast);
  }

  /**
   * Event handler for the toast's remove event.
   *
   * Not meant to be called from upstream components.
   *
   * Only invoked via the ngbToast's remove event
   * @param toastId
   */
  public _removeToast(toastId: string): void {
    this._toasts = this._toasts.filter(toast => toast.toastId !== toastId);
  }

  private _getToastStyle(toastType: ToastTypeEnum): string {
    switch (toastType) {
      case ToastTypeEnum.Success:
        return 'bg-success text-light';
      case ToastTypeEnum.Danger:
        return 'bg-danger text-light';
      case ToastTypeEnum.Info:
        return '';
    }
  }
}

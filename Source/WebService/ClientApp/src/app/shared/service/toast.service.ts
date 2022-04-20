import { Injectable } from '@angular/core';
import { ToastContainerComponent } from '../components/toast/toast-container.component';

export interface IToastInfo {
  /**
   * The header to be set for the toast
   */
  header: string,
  /**
   * The body to be set for the toast
   */
  body: string,
  /**
   * The time in ms to wait until the toast auto-hides itself.
   *
   * If nullish is provided the toast must be manually closed.
   */
  ttlMs?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private _toastContainerComponent: ToastContainerComponent;
  private _initializedWithComponentInstance: boolean = false;

  /**
   * Initialized the ToastService with an instance of the ToastContainerComponent
   * responsible for displaying and managing toasts in the UI
   * @param toastComponentInstance
   */
  public init(toastContainerComponentInstance: ToastContainerComponent): void {
    this._toastContainerComponent = toastContainerComponentInstance;
    this._initializedWithComponentInstance = true;
  }

  /**
   * Opens a success toast displaying the information provided
   * @param toast
   */
  public showSuccessToast(toast: IToastInfo): void {
    this._showToast(() => this._toastContainerComponent.openSuccessToast(toast));
  }

  /**
   * Opens a danger toast displaying the information provided
   * @param toast
   */
  public showDangerToast(toast: IToastInfo): void {
    this._showToast(() => this._toastContainerComponent.openDangerToast(toast));
  }

  private _showToast(showToastAction: () => void): void {
    if (!this._initializedWithComponentInstance)
      throw new Error('ToastContainerComponent has not initialized this service.');
    showToastAction();
  }
}

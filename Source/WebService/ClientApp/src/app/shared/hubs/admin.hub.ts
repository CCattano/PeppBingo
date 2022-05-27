import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';

enum AdminEvents {
  LatestActiveBoardID = 'LatestActiveBoardID',
  StartSetActiveBoardCooldown = 'StartSetActiveBoardCooldown',
  StartResetAllBoardsCooldown = 'StartResetAllBoardsCooldown'
}

@Injectable({
  providedIn: 'root'
})
export class AdminHub {
  private _latestBoardIDEvtCaptureDT: Date;
  private _resetAllBoardsEvtCaptureDT: Date;
  private _hubConn: HubConnection;

  private static _tryGetTimeRemainingForInProgressEventCooldown(lastEvtDateTime: Date): number {
    if (lastEvtDateTime) {
      const currentDatetime: Date = new Date();
      const cooldownExpireTime: Date =
        new Date(new Date(lastEvtDateTime).setSeconds(lastEvtDateTime.getSeconds() + 30));
      if (cooldownExpireTime > currentDatetime) {
        return Math.round(Math.abs(cooldownExpireTime.getTime() - currentDatetime.getTime()) / 1000);
      }
    }
    return null;
  }

  private static _convertDateStringsToDates(date: string): Date {
    return new Date((date).endsWith('Z') ? date : date + 'Z');
  }

  /**
   * Connect to the SignalR hub associated with this service
   *
   * If a connection has already been established,
   * invoking this does nothing.
   */
  public async connect(): Promise<void> {
    if (this._hubConn) return;
    this._hubConn =
      new HubConnectionBuilder()
        .withUrl(`${window.location.origin}/adminHub`)
        .withAutomaticReconnect()
        .build();
    await this._hubConn.start();
    this._registerInternalCooldownHandler();
  }

  /**
   * Register a callback function to act as handler for the
   * EmitLatestActiveBoardID event which broadcast a activeBoardID
   * representhing the latest active board that has been chosen by an admin
   * @param handler
   */
  public registerOnLatestActiveBoardIDHandler(handler: (activeBoardID: number) => void): void {
    this._hubConn.on(AdminEvents.LatestActiveBoardID, handler);
  }

  /**
   * Register a callback function to act as handler for the
   * EmitLatestActiveBoardID event which broadcast a activeBoardID
   * representhing the latest active board that has been chosen by an admin
   * @param handler
   */
  public registerOnStartSetActiveBoardCooldown(handler: (timeRemaining?: number) => void): void {
    this._hubConn.off(AdminEvents.StartSetActiveBoardCooldown);
    const timeRemaining: number =
      AdminHub._tryGetTimeRemainingForInProgressEventCooldown(this._latestBoardIDEvtCaptureDT);
    if (timeRemaining)
      handler(timeRemaining);
    this._hubConn.on(AdminEvents.StartSetActiveBoardCooldown, () => {
      this._latestBoardIDEvtCaptureDT = new Date();
      handler();
    });
  }

  /**
   * Register a callback function to act as handler for the
   * EmitLatestActiveBoardID event which broadcast a activeBoardID
   * representhing the latest active board that has been chosen by an admin
   *
   * Returns true or false to indicate if the hub is
   * aware of the last time a reset board event was emitted
   *
   * If false is returned last event DateTime can be fetched from the server
   * @param handler
   */
  public registerOnStartResetAllBoardsCooldown(handler: (evtDateTime: Date, timeRemaining?: number) => void): boolean {
    this._hubConn.off(AdminEvents.StartResetAllBoardsCooldown);
    const timeRemaining: number =
      AdminHub._tryGetTimeRemainingForInProgressEventCooldown(this._resetAllBoardsEvtCaptureDT);
    if (timeRemaining)
      handler(this._resetAllBoardsEvtCaptureDT, timeRemaining);
    this._hubConn.on(AdminEvents.StartResetAllBoardsCooldown, (eventStartDateTime: string) => {
      this._resetAllBoardsEvtCaptureDT = AdminHub._convertDateStringsToDates(eventStartDateTime);
      handler(this._resetAllBoardsEvtCaptureDT);
    });
    return !!this._resetAllBoardsEvtCaptureDT;
  }

  public unregisterAllHandlers(): void {
    this._hubConn.off(AdminEvents.LatestActiveBoardID);
    this._hubConn.off(AdminEvents.StartSetActiveBoardCooldown);
    this._hubConn.off(AdminEvents.StartResetAllBoardsCooldown);
    this._registerInternalCooldownHandler();
  }

  private _registerInternalCooldownHandler(): void {
    this._hubConn.on(AdminEvents.StartSetActiveBoardCooldown, () => this._latestBoardIDEvtCaptureDT = new Date());
    this._hubConn.on(AdminEvents.StartResetAllBoardsCooldown, (eventStartDateTime: string) =>
      this._resetAllBoardsEvtCaptureDT = AdminHub._convertDateStringsToDates(eventStartDateTime));
  }
}

import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import {BingoSubmissionEvent} from './events/bingo-submission.event';
import {ApproveSubmissionEvent} from './events/approve-submission.event';

enum PlayerEvents {
  LatestActiveBoardID = 'LatestActiveBoardID',
  BingoSubmission = 'BingoSubmission',
  CancelSubmission = 'CancelSubmission',
  ApproveSubmission = 'ApproveSubmission',
  RejectSubmission = 'RejectSubmission',
  ResetBoard = 'ResetBoard'
}

@Injectable({
  providedIn: 'root'
})
export class PlayerHub {
  private _hubConn: HubConnection;

  /**
   * Represents the connection id of the HubConnection on the server.
   * The connection id will be null when the connection is either in
   * the disconnected state or if the negotiation step was skipped.
   */
  public get connectionID(): string {
    return this._hubConn?.connectionId;
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
        .withUrl(`${window.location.origin}/playerHub`)
        .withAutomaticReconnect()
        .build();
    await this._hubConn.start();
  }

  public registerLatestActiveBoardIDHandler(handler: (activeBoardID: number) => void): void {
    this._hubConn.on(PlayerEvents.LatestActiveBoardID, handler);
  }

  public registerBingoSubmissionHandler(handler: (submission: BingoSubmissionEvent) => void): void {
    this._hubConn.on(PlayerEvents.BingoSubmission, handler);
  }

  public registerCancelSubmissionHandler(handler: (hubConnID: string) => void): void {
    this._hubConn.on(PlayerEvents.CancelSubmission, handler);
  }

  public registerApproveSubmissionHandler(handler: (evtData: ApproveSubmissionEvent) => void): void {
    this._hubConn.on(PlayerEvents.ApproveSubmission, handler);
  }

  public registerRejectSubmissionHandler(handler: () => void): void {
    this._hubConn.on(PlayerEvents.RejectSubmission, handler);
  }

  public registerResetBoardHandler(handler: (resetEventID: string) => void): void {
    this._hubConn.on(PlayerEvents.ResetBoard, handler);
  }

  public unregisterSubmissionResponseHandlers(): void {
    this._hubConn.off(PlayerEvents.ApproveSubmission);
    this._hubConn.off(PlayerEvents.RejectSubmission);
  }

  public unregisterAllHandlers(): void {
    this._hubConn.off(PlayerEvents.LatestActiveBoardID);
    this._hubConn.off(PlayerEvents.BingoSubmission);
    this._hubConn.off(PlayerEvents.CancelSubmission);
    this._hubConn.off(PlayerEvents.ResetBoard);
    this._hubConn.stop();
    this._hubConn = null;
  }
}

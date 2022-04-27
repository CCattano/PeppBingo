import { Component, EventEmitter, Input, Output, TemplateRef } from '@angular/core';
import { faEdit, faPlusCircle, faTrash, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { AdminApi } from '../../../../../shared/api/admin.api';
import { BoardTileDto } from '../../../../../shared/dtos/board-tile.dto';
import { ToastService } from '../../../../../shared/service/toast.service';
import { BoardTileVM } from '../../viewmodel/board-data.viewmodel';

@Component({
  selector: 'app-tile-card',
  templateUrl: './tile-card.component.html',
  styleUrls: ['../shared-tile-styles.scss']
})
export class TileCardComponent {
  @Input()
  public tile: BoardTileVM;

  /**
   * The index of the tile currently being edited
   */
  @Input()
  public index: number;

  /**
   * Event emitted when a save has occurred successfully
   *
   * Emits the index of the tile that has just been updated successfully
   */
  @Output()
  public readonly saveSuccess: EventEmitter<number> = new EventEmitter<number>();

  /**
   * Event emitted when a delete has occurred successfully
   *
   * Emits the index of the tile that has just been removed successfully
   */
  @Output()
  public readonly deleteSuccess: EventEmitter<number> = new EventEmitter<number>();

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faPlusCircle': faPlusCircle,
    'faEdit': faEdit,
    'faTrash': faTrash
  };

  constructor(
    private _modalService: NgbModal,
    private _adminApi: AdminApi,
    private _toastService: ToastService
  ) {
  }

  /**
   * Event handler for when the Edit Board button is clicked
   */
  public _onEditTileClick(): void {
    this.tile.editing = true;
  }

  /**
   * Event handler for when the IsActive toggle is clicked
   */
  public async _onIsActiveChange(): Promise<void> {
    const updatedTile: BoardTileDto =
      await this._adminApi.updateBoardTile(this.tile, false);
    Object.keys(updatedTile).forEach((key: string) =>
      (this.tile as any)[key] = (updatedTile as any)[key]);
    this.saveSuccess.emit(this.index);
  }

  /**
   * Event handler for when the delete tile icon is clicked
   * @param index
   * @param content
   */
  public async _onDeleteTileClick(modalContent: TemplateRef<any>): Promise<void> {
    const modalRef = this._modalService.open(modalContent, {
      animation: true,
      ariaLabelledBy: 'modal-basic-title',
      backdrop: 'static',
      centered: true,
      keyboard: false,
      size: 'lg'
    } as NgbModalOptions);
    const affirmativeResponse: boolean = await modalRef.result;
    if (affirmativeResponse) {
      await this._adminApi.deleteBoardTile(this.tile.tileID)
        .then(() => this.deleteSuccess.emit(this.index))
        .catch(() => this._toastService.showDangerToast({
          header: 'An error occurred',
          body: 'Tile could not be deleted. Please try again.',
          ttlMs: 3000
        }));
    }
  }
}

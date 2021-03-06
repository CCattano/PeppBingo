import { Component, EventEmitter, Input, Output, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { faArrowCircleRight, faEdit, faTrash, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { AdminApi } from '../../../../../shared/api/admin.api';
import { ToastService } from '../../../../../shared/service/toast.service';
import { BoardVM } from '../../viewmodel/board-data.viewmodel';

@Component({
  selector: 'app-board-card',
  templateUrl: './board-card.component.html',
  styleUrls: ['../shared-board-styles.scss']
})
export class BoardCardComponent {
  /**
   * The board to display data about in the template
   */
  @Input()
  public board: BoardVM;

  /**
   * The index of the board who's info we are displaying in the template
   */
  @Input()
  public index: number;

  /**
   * Event emitted when a delete has occurred successfully
   *
   * Emits the index of the tile that has just been removed successfully
   */
  @Output()
  public readonly deleteSuccess: EventEmitter<number> = new EventEmitter<number>();

  constructor(
    private _router: Router,
    private _adminApi: AdminApi,
    private _modalService: NgbModal,
    private _toastService: ToastService
  ) {
  }

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faArrowCircleRight': faArrowCircleRight,
    'faEdit': faEdit,
    'faTrash': faTrash
  };

  /**
   * Event handler for when the Edit Board button is clicked
   */
  public _onEditBoardClick(): void {
    this.board.editing = true;
  }

  public _onEditTilesClick(): void {
    this._router.navigate([
      `/admin/bingo/board/${this.board.boardID}/tiles/add-edit`
    ], {
      state: {
        boardName: this.board.name
      }
    });
  }

  /**
   * Event handler for when the delete board icon is clicked
   * @param index
   * @param content
   */
  public async _onDeleteBoardClick(modalContent: TemplateRef<any>): Promise<void> {
    const modalRef = this._modalService.open(modalContent, {
      animation: true,
      ariaLabelledBy: 'modal-basic-title',
      backdrop: 'static',
      centered: true,
      keyboard: false,
      size: 'lg'
    } as NgbModalOptions);
    const affirmativeResponse: boolean = await modalRef.result;
    if (affirmativeResponse)
      await this._adminApi.deleteBoard(this.board.boardID)
        .then(() => this.deleteSuccess.emit(this.index))
        .catch(() => this._toastService.showDangerToast({
          header: 'An Error Occurred',
          body: 'Board could not be deleted. Please try again.',
          ttlMs: 5000
        }));
  }
}

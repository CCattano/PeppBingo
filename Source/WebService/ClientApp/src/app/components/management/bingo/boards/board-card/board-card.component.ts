import { Component, Input, TemplateRef } from '@angular/core';
import { faArrowCircleRight, faEdit, faTrash, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { BoardVM } from '../viewmodels/board.viewmodel';

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

  constructor(private _modalService: NgbModal) {
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
    if (affirmativeResponse) {
      alert('Delete goes here');
    } else {
      alert('The board lives to die another day');
    }
  }
}

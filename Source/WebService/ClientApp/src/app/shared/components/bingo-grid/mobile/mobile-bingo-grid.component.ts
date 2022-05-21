import {Component} from '@angular/core';
import {BingoGridAbstract} from '../bingo-grid-abstract';
import * as html2canvas from 'html2canvas';

@Component({
  selector: 'app-mobile-bingo-grid',
  templateUrl: './mobile-bingo-grid.component.html',
  styleUrls: [
    '../bingo-grid.styles.scss',
  ]
})
export class MobileBingoGridComponent extends BingoGridAbstract {
  public getScreenshotOfBoard(): void {
    const div: HTMLDivElement = document.createElement('div');
    div.style.width = '100%';
    div.style.height = '100%';
    div.style.position = 'fixed';
    div.style.top = '500%';
    const boardClone = document.getElementById('bingoBoard').cloneNode(true) as HTMLDivElement;
    boardClone.classList.remove('d-none');
    div.append(boardClone);
    window.document.body.append(div);
    // @ts-ignore
    html2canvas(boardClone, {backgroundColor: '#1c1f23'}).then(canvas => {
      div.parentElement.removeChild(div);
      const win: Window = window.open('', '_blank');
      win.document.write(`<iframe src="${canvas.toDataURL()}" style="border:0; top:0; left:0; bottom:0; right:0; width:100%; height:100%;" allowfullscreen></iframe>`);
    });
  }
}

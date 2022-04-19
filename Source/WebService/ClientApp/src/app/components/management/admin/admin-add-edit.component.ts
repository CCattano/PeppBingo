import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { faMinusCircle, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject, Observable, of, Subscription } from 'rxjs';
import { debounceTime, filter, map, switchMap, tap } from 'rxjs/operators';
import { UserApi } from '../../../shared/api/user.api';
import { UserDto } from '../../../shared/dtos/user.dto';
import { UserSearchResultVM } from './viewmodels/user-search-result.viewmodel';

@Component({
  selector: 'app-admin-add-edit',
  templateUrl: './admin-add-edit.component.html',
  styleUrls: ['./admin-add-edit.component.scss']
})
export class AddEditAdminComponent implements OnInit, OnDestroy {
  //#region UserSearch Pipeline Variables

  private _inputChangeSubscription: Subscription;
  private _inputChangeSource: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  private _inputChange$: Observable<string> = this._inputChangeSource.asObservable();
  /**
   * Search results containing users that can be added as admins
   */
  public _searchResults: UserSearchResultVM[] = [];
  /**
   * Bool flag indicating a search returned no results
   */
  public _noSearchResults: boolean = false;

  //#endregion

  //#region Current Admin Variables

  /**
   * Fontawesome icon used to initiate the remove user process
   */
  public _faMinusCircle: IconDefinition = faMinusCircle;

  /**
   * Users marked as Administrators in the system
   */ 
  public _admins: UserDto[] = [];

  /**
   * Data referenced by the add/remove admin modals
   */
  public _userModalData: UserDto;

  //#enregion

  constructor(
    private _userApi: UserApi,
    private _modalService: NgbModal) {
  }

  //#region Lifecycle Hooks

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    this._inputChangeSubscription = this._initInputValueChangePipeline().subscribe();
    this._admins = await this._userApi.getAdmins();
  }

  /**
   * @inheritdoc
   */
  public ngOnDestroy(): void {
    this._inputChangeSubscription?.unsubscribe();
    this._inputChangeSubscription = null;
  }

  //#endregion

  /**
   * Event handler for the input changed event
   * Emits the latest value on the input to our
   * user search pipeline
   * @param event
   */
  public _onInput(event: Event): void {
    event.stopPropagation();
    this._inputChangeSource.next((event.target as HTMLInputElement).value);
  }

  //#region UserSearch Pipeline Functions

  /**
   * Blur event handler for the User Search input
   * Clears search results and empties input to
   * prepare for next search to perform
   */
  public _onBlur(event: Event): void {
    (event.target as HTMLInputElement).value = '';
    const inputEvent: Event = new Event('input', { bubbles: true, cancelable: true });
    (event.target as HTMLInputElement).dispatchEvent(inputEvent);
  }

  private _initInputValueChangePipeline(): Observable<void> {
    return this._inputChange$.pipe(
      debounceTime(250),
      filter((searchTerm: string) => {
        const shouldContinue: boolean = !!searchTerm && searchTerm.length >= 2;
        if (!shouldContinue)
          this._searchResults = [];
        return shouldContinue;
      }),
      switchMap((searchTerm: string) => of(null).pipe(
        switchMap(() => this._userApi.searchUsersByName(searchTerm).catch(() => [])),
        map((users: UserDto[]) => users?.filter(user => !user.isAdmin) ?? []),
        map((users: UserDto[]) => [searchTerm, users] as [string, UserDto[]])
      )),
      map(([searchTerm, users]: [string, UserDto[]]) => users?.map(user => {
        let name: string = user.displayName;
        const matchPattern: RegExp = new RegExp(searchTerm, 'gi');
        const matches: string[] = name.match(matchPattern);
        matches.forEach(match => {
          const highlightedText: string = `<span style="color:#fed800">${match}</span>`;
          const replacePattern: RegExp = new RegExp(match, 'g');
          name = name.replace(replacePattern, highlightedText);
        });
        return { ...user, highlightedName: name } as UserSearchResultVM;
      }) ?? []),
      tap((users: UserSearchResultVM[]) => {
        this._searchResults = users;
        this._noSearchResults = !this._searchResults?.length;
      }),
      map(() => null)
    );
  }

  //#endregion

  //#region Current Admin Functions

  /**
   * Revoke a user's admin permissions within the application
   * @param index
   */
  public async _onRemoveAdminClick(index: number, content: TemplateRef<any>): Promise<void> {
    this._userModalData = this._admins[index];
    const affirmativeAction =
      async () =>
        await this._userApi.revokeAdminPermissionForUser(this._admins[index].userID)
          .then(() => this._admins.splice(index, 1) && null)
          .catch(() => alert('Could not remove user. Please try again.'));
    await this._openModal(index, content, affirmativeAction);
  }

  /**
   * Grant a user admin permissions within the application
   * @param index
   */
  public async _onAddAdminClick(index: number, content: TemplateRef<any>): Promise<void> {
    this._userModalData = { ...this._searchResults[index] } as UserDto;
    const affirmativeAction =
      async () => 
        await this._userApi.GrantAdminPermissionForUser(this._userModalData.userID)
          .then(() => this._admins.push(this._userModalData) && null)
          .catch(() => alert('Could not add user. Please try again.'));
    await this._openModal(index, content, affirmativeAction);
  }

  private async _openModal(index: number, content: TemplateRef<any>, affirmativeAction: () => Promise<void>) {
    const modalRef = this._modalService.open(content, {
      animation: true,
      ariaLabelledBy: 'modal-basic-title',
      backdrop: 'static',
      centered: true,
      keyboard: false,
      size: 'lg'
    } as NgbModalOptions);
    const affirmativeResponse: boolean = await modalRef.result;
    if (affirmativeResponse) {
      await affirmativeAction();
    }
  }

  //#endregion
}

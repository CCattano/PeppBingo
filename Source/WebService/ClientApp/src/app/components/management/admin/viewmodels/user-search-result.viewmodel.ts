import { UserDto } from '../../../../shared/dtos/user.dto';

export class UserSearchResultVM extends UserDto {
  /**
   * String containing highlighting for portions
   * that matched the search that was performed.
   */ 
  public highlightedName: string;
}

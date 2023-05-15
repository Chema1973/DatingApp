import { Injectable } from '@angular/core';
import { CanDeactivate,  } from '@angular/router';
import { Observable, of } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from '../_services/confirm.service';

@Injectable({
  providedIn: 'root'
})
// TODO
// CanDeactivate desprecated
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
  constructor(private confirmService: ConfirmService) {}
  // canDeactivate(component: MemberEditComponent): boolean {
    canDeactivate(component: MemberEditComponent): Observable<boolean>{
    if (component.editForm?.dirty) {
      // return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
      return this.confirmService.confirm()
    }
    // return true;
    return of(true);
  }
  
}

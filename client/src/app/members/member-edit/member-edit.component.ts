import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any){
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  // --> Esto es para evitar los botones de navegación (refresh) y que salga un aviso
  member: Member | undefined;
  user: User | null = null;

  constructor(private accountService: AccountService, private memberService: MembersService, private toastrService: ToastrService){
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        this.user = user;
        console.log(this.user)
      }
    })
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    console.log('loadMember::1');
    console.log(this.user);
    if (!this.user) return;
    console.log('loadMember::2::' + this.user.userName);
    this.memberService.getMember(this.user.userName).subscribe({
      next: member => {
        this.member = member;
        console.log(this.member)
      }
    })
  }

  updateMember() {
    console.log('updateMember');
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.toastrService.success('Profile updated successfully');
        this.editForm?.reset(this.member);
      }
    })
    
  }

}

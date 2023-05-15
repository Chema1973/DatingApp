import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() member: Member | undefined; //: Member = {} as Member;

  constructor(private memberService: MembersService, private toastrSevice: ToastrService,
    public presenceService: PresenceService){}

  ngOnInit(): void {
    console.log(this.presenceService.onlineUsers$);
    console.log(this.member);
  }

  addLike(member: Member){
    this.memberService.addLike(member.username).subscribe({
      next: () => this.toastrSevice.success('You have liked ' + member.knownAs)
    })
  }
}

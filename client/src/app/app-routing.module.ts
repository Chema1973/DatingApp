import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailedResolver } from './_resolvers/member-detailed.resolver';
// NAVEGACIÓN
const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: '',
      runGuardsAndResolvers: 'always',
      canActivate: [AuthGuard],
      children: [ // A todos los "children" activamos el "AuthGuard"
        // { path: 'members/:id', component: MemberDetailComponent},
        { path: 'members/:username', component: MemberDetailComponent, resolve: {member: MemberDetailedResolver}},
        // { path: 'members/:username', component: MemberDetailComponent},
        { path: 'member/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard]}, // Añadimos un "guardia" para desactivar la navegación si ha hecho cambios en el formulario
        { path: 'lists', component: ListsComponent},
        { path: 'messages', component: MessagesComponent},
        { path: 'admin', component: AdminPanelComponent, canActivate: [AdminGuard]},
      ]
  },
  { path: 'members', component: MemberListComponent, canActivate: [AuthGuard]}, // Activamos el "AuthGuard" en una ruta específica
  { path: 'not-found', component: NotFoundComponent},
  { path: 'server-error', component: ServerErrorComponent},
  { path: 'errors', component: TestErrorComponent},
  { path: '**', component: NotFoundComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

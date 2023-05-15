import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr/dist/esm/HubConnection';
import { HubConnectionBuilder } from '@microsoft/signalr/dist/esm/HubConnectionBuilder';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Group } from '../_models/group';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();
  // --> Creamos un Observable para la comunicación entre dos usuarios

  constructor(private http: HttpClient) { }

  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();
      this.hubConnection.start().catch(error => console.log(error));
      // Son los identificadores de "APIDatingApp\SignalR\MessageHub.cs"

      this.hubConnection.on('ReceiveMessageThread', messages => {
        this.messageThreadSource.next(messages);
      }); 

      this.hubConnection.on('UpdatedGroup', (group: Group) => {
        if (group.connections.some(x => x.username === otherUsername))
        {
          this.messageThread$.pipe(take(1)).subscribe({
            next: messages => {
              messages.forEach(message => {
                if (!message.dateRead) {
                  message.dateRead = new Date(Date.now());
                }
              })
              this.messageThreadSource.next([...messages]);
            }
          });
        }
      }); 

      this.hubConnection.on('NewMessage', message => {
        this.messageThread$.pipe(take(1)).subscribe({
          next: messages => {
            this.messageThreadSource.next([...messages, message]);
            // this.messageThreadSource.next([messages, message]);
          }
        })
      });
      // OJO:: A este se le hace el "on" en "presence.service.cs"
      // this.hubConnection.on('NewMessageReceived', user => {});

  }

  stopHubConnection() {

    if (this.hubConnection) {
      this.hubConnection.stop();
    }
    // this.hubConnection?.stop().catch(error => console.log(error));
  }

  getMessages(pageNumber: number, pageSize: number, container: string) 
  {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }
  
  getMessagesThread(username: string) {
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
  }

  async sendMessage(username: string, content: string) {
    // return this.http.post<Message>(this.baseUrl + 'messages', {recipientUserName: username, content: content});
    console.log(this.hubConnection);

// TODO
// ÑAPAQUI : 
/*
Error: Failed to invoke 'SendMessage' due to an error on the server. HubException: Method does not exist.
    at _callbacks.<computed> (HubConnection.js:312:36)
    at HubConnection._processIncomingData (HubConnection.js:420:33)
    at HubConnection.connection.onreceive (HubConnection.js:42:52)
    at webSocket.onmessage [as __zone_symbol__ON_PROPERTYmessage] (WebSocketTransport.js:81:30)
    at WebSocket.wrapFn (zone.js:769:39)
    at _ZoneDelegate.invokeTask (zone.js:409:31)
    at core.mjs:23896:55
    at AsyncStackTaggingZoneSpec.onInvokeTask (core.mjs:23896:36)
    at _ZoneDelegate.invokeTask (zone.js:408:60)
    at Object.onInvokeTask (core.mjs:24197:33)

*/

    return this.hubConnection?.invoke('SendMessage', {recipientUserName: username, content})
      .catch(error => console.log(error));
    // "SendMessage" de "APIDatingApp\SignalR\MessageHub.cs", se tiene que llamar exactamente igual
    // Lo hacemos "async" porque devuelve una "promesa"
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}

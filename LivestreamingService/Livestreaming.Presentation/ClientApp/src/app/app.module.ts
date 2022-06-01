import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LivestreamSetupComponent } from './livestream-setup/livestream-setup.component';
import { LivestreamWatchComponent } from './livestream-watch/livestream-watch.component';
import { LivestreamChatComponent } from './livestream-chat/livestream-chat.component';
import { LivestreamHistoryComponent } from './livestream-history/livestream-history.component';
import { LivestreamHowtoComponent } from './livestream-howto/livestream-howto.component';
import { LivestreamStatusComponent } from './livestream-status/livestream-status.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { VjsPlayerComponent } from './vjs-player/vjs-player.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LivestreamSetupComponent,
    LivestreamWatchComponent,
    LivestreamChatComponent,
    LivestreamHistoryComponent,
    LivestreamHowtoComponent,
    LivestreamStatusComponent,
    VjsPlayerComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatExpansionModule,
    MatCardModule,
    MatDividerModule,
    MatProgressBarModule,
    MatGridListModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'livestreams/setup', pathMatch: 'full' },
      { path: 'livestreams/setup', component: LivestreamSetupComponent },
      { path: 'livestreams/howto', component: LivestreamHowtoComponent },
      { path: 'livestreams/history/:userId/:page', component: LivestreamHistoryComponent },
      { path: 'livestreams/:id/watch', component: LivestreamWatchComponent },
      { path: 'livestreams/:id/chat', component: LivestreamChatComponent },
      { path: 'livestreams/:id/status', component: LivestreamStatusComponent }
    ]),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

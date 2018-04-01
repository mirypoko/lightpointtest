import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Material
import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatFormFieldModule,

  ErrorStateMatcher,
  ShowOnDirtyErrorStateMatcher,
} from '@angular/material';

// Components.
import { FooterComponent } from './components/core/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { SingInComponent } from './components/user/sing-in/sing-in.component';
import { SingUpComponent } from './components/user/sing-up/sing-up.component';
import { PageNotFoundComponent } from './components/core/page-not-found/page-not-found.component';
import { AppComponent } from './components/core/app/app.component';
import { UserMenuComponent } from './components/user/user-menu/user-menu.component';
import { SettingsComponent } from './components/user/settings/settings.component';
import { CounterComponent } from './components/counter/counter.component';
import { FetchDataComponent } from './components/fetch-data/fetch-data.component';
import { DetailsProductComponent } from './components/products/details-product/details-product.component';

// Validators.
import { EqualValidator } from './validators/equal.validator';

// Servises.
import { AuthenticationService } from './services/authentication-service.service';
import { UserService } from './services/user.service';
import { SnackBarsServiceService } from './services/snack-bars-service.service';

// Interceptors.
import { AuthenticationInterceptor } from './interceptors/authentication.interceptor';
import { JwtInterceptor } from './interceptors/jwtInterceptor.interceptor';

// Guards.
import { AuthorizationGuard } from './guards/authorization.guard';
import { MainSidenavComponent } from './components/core/main-sidenav/main-sidenav.component';

import { AppRoutingModule } from './app-routing.module';
import { ListShopComponent } from './components/shops/list-shop/list-shop.component';
import { AddShopComponent } from './components/shops/add-shop/add-shop.component';
import { ShopsService } from './services/shops.service';
import { AdminGuard } from './guards/admin.guard';
import { ListProductsComponent } from './components/products/list-products/list-products.component';
import { ProductsService } from './services/products.service';
import { AddProductComponent } from './components/products/add-product/add-product.component';

@NgModule({
  declarations: [

    // Components
    AppComponent,
    FooterComponent,
    HomeComponent,
    SingInComponent,
    SingUpComponent,
    PageNotFoundComponent,
    UserMenuComponent,
    SettingsComponent,
    MainSidenavComponent,
    CounterComponent,
    FetchDataComponent,
    AddShopComponent,
    ListShopComponent,
    ListProductsComponent,
    AddProductComponent,
    DetailsProductComponent,

    // Validators
    EqualValidator,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
     // Material
     MatFormFieldModule,
     MatAutocompleteModule,
     MatButtonModule,
     MatButtonToggleModule,
     MatCardModule,
     MatCheckboxModule,
     MatChipsModule,
     MatStepperModule,
     MatDatepickerModule,
     MatDialogModule,
     MatDividerModule,
     MatExpansionModule,
     MatGridListModule,
     MatIconModule,
     MatInputModule,
     MatListModule,
     MatMenuModule,
     MatNativeDateModule,
     MatPaginatorModule,
     MatProgressBarModule,
     MatProgressSpinnerModule,
     MatRadioModule,
     MatRippleModule,
     MatSelectModule,
     MatSidenavModule,
     MatSliderModule,
     MatSlideToggleModule,
     MatSnackBarModule,
     MatSortModule,
     MatTableModule,
     MatTabsModule,
     MatToolbarModule,
     MatTooltipModule,
 
     BrowserAnimationsModule,
 
     BrowserModule,
     AppRoutingModule,
 
     FormsModule,
     ReactiveFormsModule,
 
     HttpClientModule
  ],
  providers: [
    {
      provide: ErrorStateMatcher,
      useClass: ShowOnDirtyErrorStateMatcher
    },
    // Services
    UserService,
    SnackBarsServiceService,
    AuthenticationService,
    ShopsService,
    ProductsService,
    // Interceptors
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    // Guards
    AuthorizationGuard,
    AdminGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

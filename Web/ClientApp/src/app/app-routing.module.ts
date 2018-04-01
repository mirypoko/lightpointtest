import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { SingInComponent } from './components/user/sing-in/sing-in.component';
import { PageNotFoundComponent } from './components/core/page-not-found/page-not-found.component';
import { SingUpComponent } from './components/user/sing-up/sing-up.component';
import { SettingsComponent } from './components/user/settings/settings.component';
import { AuthorizationGuard } from './guards/authorization.guard';
import { CounterComponent } from './components/counter/counter.component';
import { FetchDataComponent } from './components/fetch-data/fetch-data.component';
import { ListShopComponent } from './components/shops/list-shop/list-shop.component';
import { AddShopComponent } from './components/shops/add-shop/add-shop.component';
import { AdminGuard } from './guards/admin.guard';
import { ListProductsComponent } from './components/products/list-products/list-products.component';
import { AddProductComponent } from './components/products/add-product/add-product.component';
import { DetailsProductComponent } from './components/products/details-product/details-product.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'products', component: ListProductsComponent },
  { path: 'products/:shopId', component: ListProductsComponent },
  { path: 'add-product', component: AddProductComponent, canActivate: [AuthorizationGuard, AdminGuard]  },
  { path: 'dtails-product/:id', component: DetailsProductComponent },
  { path: 'shops', component: ListShopComponent },
  { path: 'add-shop', component: AddShopComponent, canActivate: [AuthorizationGuard, AdminGuard]  },
  { path: 'settings', component: SettingsComponent, canActivate: [AuthorizationGuard] },
  { path: 'singin', component: SingInComponent },
  { path: 'singup', component: SingUpComponent },
  { path: '**', component: PageNotFoundComponent },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: '404', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

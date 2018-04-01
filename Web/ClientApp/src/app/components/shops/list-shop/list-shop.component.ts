import { Component, OnInit, ViewChild } from '@angular/core';
import { ShopsService } from '../../../services/shops.service';
import { PageEvent, MatTableDataSource, MatPaginator, MatSnackBarConfig } from '@angular/material';
import { IShop } from '../../../models/shop';
import { HttpErrorResponse } from '@angular/common/http';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';
import { AuthenticationService } from '../../../services/authentication-service.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-list-shop',
  templateUrl: './list-shop.component.html',
  styleUrls: ['./list-shop.component.css']
})
export class ListShopComponent {

  currentUserIsAdmin: boolean = false;

  currentUserSubscription: Subscription;

  displayedColumns = ['name', 'address', 'shopMode', 'actions'];
  dataSource: MatTableDataSource<IShop>;

  filter: string = '';

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private _shopsService: ShopsService,
    private _snackBarsService: SnackBarsServiceService,
    private _authService: AuthenticationService) {
    this.currentUserSubscription = this._authService.CurrentUser.subscribe(
      result => {
        this._authService.currentUserIsAdmin().subscribe(
          sub =>
            this.currentUserIsAdmin = sub
        );
      }
    );
  }

  ngAfterViewInit() {
    this._shopsService.get().subscribe(
      items => {
        this.dataSource = new MatTableDataSource(items);
      },
      (error: HttpErrorResponse) => {
        this.showServerErrorSnackBar(error);
      }
    );
  }

  delete(id: number) {
    if (confirm('Are you sure?')) {
      this._shopsService.delete(id).subscribe(
        result => {
          this.showSuccessSnackBar('Deleted');
          this.ngAfterViewInit();
        },
        error => this.showServerErrorSnackBar(error)
      );
    }
  }


  applyFilter() {
    this.filter = this.filter.trim();
    this.dataSource.filter = this.filter;
  }

  protected showSuccessSnackBar(message: string) {
    this._snackBarsService.showSuccess(message)
  }

  showServerErrorSnackBar(error: HttpErrorResponse) {
    let errorString = 'Server error ' + error.status + ': ' + error.statusText;
    this._snackBarsService.showError(errorString)
  }

}

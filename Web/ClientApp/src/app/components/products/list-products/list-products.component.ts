import { Component, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../../../models/product';
import { MatTableDataSource, PageEvent, MatPaginator } from '@angular/material';
import { ProductsService } from '../../../services/products.service';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../../../services/authentication-service.service';
import { Subscription } from 'rxjs';
import { ShopsService } from '../../../services/shops.service';
import { IShop } from '../../../models/shop';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-list-products',
  templateUrl: './list-products.component.html',
  styleUrls: ['./list-products.component.css']
})
export class ListProductsComponent implements OnInit {

  currentUserIsAdmin: boolean = false;

  shops: Array<IShop> = [{ name: "All", id: 0, address: null, shopModeId: -1, shopMode: null }];

  shopFilterId = 0;

  private sub: any;

  currentUserSubscription: Subscription;

  displayedColumns = ['imgUrl', 'name', 'description', 'shop', 'actions'];
  dataSource: MatTableDataSource<IProduct>;

  filter: string = '';

  length = 100;
  pageSize = 3;
  pageSizeOptions = [3, 10, 25, 100];
  pageEvent: PageEvent;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private _route: ActivatedRoute,
    private _productService: ProductsService,
    private _snackBarsService: SnackBarsServiceService,
    private _authService: AuthenticationService,
    private _shopsService: ShopsService) {
    this.currentUserSubscription = this._authService.CurrentUser.subscribe(
      result => {
        this._authService.currentUserIsAdmin().subscribe(
          sub =>
            this.currentUserIsAdmin = sub
        );
      }
    );
    this.sub = this._route.params.subscribe(params => {
      this.shopFilterId = +params['shopId'];
      if (this.shopFilterId > 0) {
      }
    });
  }

  ngOnInit() {

    this._shopsService.get().subscribe(
      items => {
        for (let i = 0; i < items.length; i++) {
          this.shops[i + 1] = items[i];
        }
      },
      error => this.showServerErrorSnackBar(error)
    )
  }

  ngAfterViewInit() {
    if (this.shopFilterId > 0) {
      this._productService.search(this.pageSize, this.filter, 0, this.shopFilterId).subscribe(
        items => {
          for (let i = 0; i < items.length; i++) {
            if (items[i].description.length > 200) {
              items[i].description = items[i].description.substr(0, 200) + '...';
            }
          }
          this.dataSource = new MatTableDataSource(items);
          this._productService.getCountFilter(this.filter, this.shopFilterId).subscribe(
            count => {
              this.length = count;
            },
            (error: HttpErrorResponse) => {
              this.showServerErrorSnackBar(error);
            }
          );
        },
        (error: HttpErrorResponse) => {
          this.showServerErrorSnackBar(error);
        }
      );
    } else {
      this._productService.get(this.pageSize).subscribe(
        items => {
          for (let i = 0; i < items.length; i++) {
            if (items[i].description.length > 200) {
              items[i].description = items[i].description.substr(0, 200) + '...';
            }
          }
          this.dataSource = new MatTableDataSource(items);
          this.dataSource.paginator = this.paginator;
          this._productService.getCount().subscribe(
            count => {
              this.length = count;
            },
            (error: HttpErrorResponse) => {
              this.showServerErrorSnackBar(error);
            }
          );
        },
        (error: HttpErrorResponse) => {
          this.showServerErrorSnackBar(error);
        }
      );
    }
  }

  delete(id: number) {
    if (confirm('Are you sure?')) {
      this._productService.delete(id).subscribe(
        result => {
          this.showSuccessSnackBar('Deleted');
          this.ngAfterViewInit();
        },
        error => this.showServerErrorSnackBar(error)
      );
    }
  }

  onPageEvent() {
    if (this.filter.length > 2 || this.shopFilterId > 0) {
      this._productService.search(this.pageEvent.pageSize, this.filter, this.pageEvent.pageIndex * this.pageEvent.pageSize, this.shopFilterId).subscribe(
        items => {
          for (let i = 0; i < items.length; i++) {
            if (items[i].description.length > 200) {
              items[i].description = items[i].description.substr(0, 200) + '...';
            }
          }
          this.dataSource = new MatTableDataSource(items);
        },
        (error: HttpErrorResponse) => {
          this.showServerErrorSnackBar(error);
        }
      );
    } else {
      this._productService.get(this.pageEvent.pageSize, this.pageEvent.pageIndex * this.pageEvent.pageSize).subscribe(
        items => {
          for (let i = 0; i < items.length; i++) {
            if (items[i].description.length > 200) {
              items[i].description = items[i].description.substr(0, 200) + '...';
            }
          }
          this.dataSource = new MatTableDataSource(items);
        },
        (error: HttpErrorResponse) => {
          this.showServerErrorSnackBar(error);
        }
      );
    }
  }

  applyFilter() {
    let filter = this.filter.trim(); // Remove whitespace
    filter = this.filter.toLowerCase(); // Datasource defaults to lowercase matches

    if (filter.length == 0 && this.shopFilterId == 0) {
      this._productService.get(this.pageSize).subscribe(
        items => {
          for (let i = 0; i < items.length; i++) {
            if (items[i].description.length > 200) {
              items[i].description = items[i].description.substr(0, 200) + '...';
            }
          }
          this.dataSource = new MatTableDataSource(items);
          this._productService.getCount().subscribe(
            count => {
              this.length = count;
            },
            (error: HttpErrorResponse) => {
              this.showServerErrorSnackBar(error);
            }
          );
        },
        (error: HttpErrorResponse) => {
          this.showServerErrorSnackBar(error);
        }
      );
      return;
    }

    if (filter.length < 2 && this.shopFilterId == 0) return;

    this._productService.search(this.pageSize, filter, 0, this.shopFilterId).subscribe(
      items => {
        for (let i = 0; i < items.length; i++) {
          if (items[i].description.length > 200) {
            items[i].description = items[i].description.substr(0, 200) + '...';
          }
        }
        this.dataSource = new MatTableDataSource(items);
        this._productService.getCountFilter(filter, this.shopFilterId).subscribe(
          count => {
            this.length = count;
          },
          (error: HttpErrorResponse) => {
            this.showServerErrorSnackBar(error);
          }
        );
      },
      (error: HttpErrorResponse) => {
        this.showServerErrorSnackBar(error);
      }
    );
  }

  protected showSuccessSnackBar(message: string) {
    this._snackBarsService.showSuccess(message)
  }

  showServerErrorSnackBar(error: HttpErrorResponse) {
    let errorString = 'Server error ' + error.status + ': ' + error.statusText;
    this._snackBarsService.showError(errorString)
  }

  ngOnDestroy(): void {
    this.currentUserSubscription.unsubscribe();
  }
}

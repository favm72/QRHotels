import { BrowserModule } from "@angular/platform-browser"
import { LOCALE_ID, NgModule } from "@angular/core"
import { FormsModule, ReactiveFormsModule } from "@angular/forms"
import { AppComponent } from "./app.component"
import { RouterModule, Routes } from "@angular/router"

import { FontAwesomeModule } from "@fortawesome/angular-fontawesome"

import { LoginComponent } from "./components/login/login.component"
import { MainComponent } from "./components/main/main.component"
import { VisorComponent } from "./components/visor/visor.component"
import { MasterComponent } from "./components/master/master.component"
import { CartComponent } from "./components/cart/cart.component"
import { AddtocartComponent } from "./components/addtocart/addtocart.component"
import { SendorderComponent } from "./components/sendorder/sendorder.component"
import { ServicesComponent } from "./components/services/services.component"
import { DigitalComponent } from "./components/digital/digital.component"
import { AlertComponent } from "./components/utils/alert/alert.component"
import { LoginAdminComponent } from "./components/login-admin/login-admin.component"
import { MasterAdminComponent } from "./components/master-admin/master-admin.component"
import { MainAdminComponent } from "./components/main-admin/main-admin.component"
import { OrderDetailComponent } from "./components/order-detail/order-detail.component"
import { TimerComponent } from "./components/utils/timer/timer.component"
import { OrderItemComponent } from "./components/order-item/order-item.component"
import { LoadingComponent } from "./components/utils/loading/loading.component"
import { ProductComponent } from "./components/cart/product/product.component"
import { CategoryComponent } from "./components/cart/category/category.component"
import { CountmarkComponent } from "./components/utils/countmark/countmark.component"
import { MyOrdersComponent } from "./components/my-orders/my-orders.component"
import { MyDetailComponent } from "./components/my-detail/my-detail.component"
import { OnlyNumberDirective } from "./directives/only-number.directive"
import { SliderComponent } from "./components/utils/slider/slider.component"
import { DetailComponent } from "./components/detail/detail.component"
import { ModalComponent } from "./components/utils/modal/modal.component"
import locale from "@angular/common/locales/es-PE"
import { registerLocaleData } from "@angular/common"
import { AdminGuard } from "./guards/admin.guard"
import { ClientGuard } from "./guards/client.guard"
import { DirectoryComponent } from "./components/directory/directory.component"
import { AccordionComponent } from "./components/utils/accordion/accordion.component"
import { RawhtmlPipe } from "./pipes/rawhtml.pipe"
import { LateCheckoutComponent } from "./components/late-checkout/late-checkout.component"
import { AdminProductsComponent } from "./components/admin-products/admin-products.component"
import { TooltipComponent } from "./components/utils/tooltip/tooltip.component"
import { ViewCartComponent } from "./components/view-cart/view-cart.component"
import { NgMultiSelectDropDownModule } from "ng-multiselect-dropdown"
import { AdminCartComponent } from "./components/admin-cart/admin-cart.component"
import { InputTimeDirective } from "./directives/input-time.directive"
import { ProductManagerComponent } from "./components/product-manager/product-manager.component"
import { SwitchComponent } from "./components/utils/switch/switch.component"
import { UploaderComponent } from "./components/utils/uploader/uploader.component"
import { IntakeComponent } from "./components/intake/intake.component"
import { IntakeDetailComponent } from "./components/intake-detail/intake-detail.component"
import { OnlyDecimalDirective } from "./directives/only-decimal.directive"
import { SendMailComponent } from "./components/send-mail/send-mail.component"
import { AdminHomeComponent } from "./components/admin-home/admin-home.component"
import { AdminDirectoryComponent } from "./components/admin-directory/admin-directory.component"
import { QuillModule } from "ngx-quill"
import { AmmenitiesComponent } from "./components/ammenities/ammenities.component"
import { AdminInfoPagesComponent } from "./components/admin-info-pages/admin-info-pages.component"
import { InformativeComponent } from "./components/informative/informative.component"
import { LimpiezaComponent } from "./components/limpieza/limpieza.component"
registerLocaleData(locale, "pe")

const routes: Routes = [
	{ path: "", pathMatch: "full", redirectTo: "login/CASA-MIRAFLORESPREMIUM" },
	{ path: "login/:hotelCode", component: LoginComponent, pathMatch: "full" },
	{ path: "loginadmin", component: LoginAdminComponent, pathMatch: "full" },
	{
		path: "admin",
		component: MasterAdminComponent,
		canActivateChild: [AdminGuard],
		children: [
			{ path: "", redirectTo: "main", pathMatch: "full" },
			{ path: "main", component: MainAdminComponent },
			{ path: "detail", component: OrderDetailComponent },
			{ path: "products", component: AdminProductsComponent },
			{ path: "productmgr", component: ProductManagerComponent },
			{ path: "cart", component: AdminCartComponent },
			{ path: "homemenu", component: AdminHomeComponent },
			{ path: "directory", component: AdminDirectoryComponent },
			{ path: "infoPage", component: AdminInfoPagesComponent },
		],
	},
	{
		path: "master",
		component: MasterComponent,
		canActivateChild: [ClientGuard],
		children: [
			{ path: "", redirectTo: "main", pathMatch: "full" },
			{ path: "main", component: MainComponent },
			{ path: "myorders", component: MyOrdersComponent },
			{ path: "detail", component: MyDetailComponent },
			{ path: "services", component: ServicesComponent },
			{ path: "directory", component: DirectoryComponent },
			{ path: "digital", component: DigitalComponent },
			{ path: "cocinaacasa", component: VisorComponent },
			{ path: "embed", component: VisorComponent },
			{ path: "latecheckout", component: LateCheckoutComponent },
			{ path: "experiencia", component: VisorComponent },
			{ path: "ammenities", component: AmmenitiesComponent },
			{ path: "informative", component: InformativeComponent },
			{ path: "limpieza", component: LimpiezaComponent },
		],
	},
	{ path: "intake", component: IntakeComponent, canActivate: [ClientGuard] },
	{
		path: "intakedetail",
		component: IntakeDetailComponent,
		canActivate: [ClientGuard],
	},
	{
		path: "sendmail",
		component: SendMailComponent,
		canActivate: [ClientGuard],
	},
	{ path: "cart", component: CartComponent, canActivate: [ClientGuard] },
	{
		path: "addtocart",
		component: AddtocartComponent,
		canActivate: [ClientGuard],
	},
	{ path: "order", component: ViewCartComponent, canActivate: [ClientGuard] },
	{
		path: "order-confirm",
		component: SendorderComponent,
		canActivate: [ClientGuard],
	},
]

@NgModule({
	declarations: [
		AppComponent,
		LoginComponent,
		MainComponent,
		VisorComponent,
		MasterComponent,
		CartComponent,
		AddtocartComponent,
		SendorderComponent,
		ServicesComponent,
		DigitalComponent,
		AlertComponent,
		LoginAdminComponent,
		MasterAdminComponent,
		MainAdminComponent,
		OrderDetailComponent,
		TimerComponent,
		OrderItemComponent,
		LoadingComponent,
		ProductComponent,
		CategoryComponent,
		CountmarkComponent,
		MyOrdersComponent,
		MyDetailComponent,
		OnlyNumberDirective,
		SliderComponent,
		DetailComponent,
		ModalComponent,
		DirectoryComponent,
		AccordionComponent,
		RawhtmlPipe,
		LateCheckoutComponent,
		AdminProductsComponent,
		TooltipComponent,
		ViewCartComponent,
		AdminCartComponent,
		InputTimeDirective,
		ProductManagerComponent,
		SwitchComponent,
		UploaderComponent,
		IntakeComponent,
		IntakeDetailComponent,
		OnlyDecimalDirective,
		SendMailComponent,
		AdminHomeComponent,
		AdminDirectoryComponent,
		AmmenitiesComponent,
		AdminInfoPagesComponent,
		InformativeComponent,
		LimpiezaComponent,
	],
	imports: [
		ReactiveFormsModule,
		FormsModule,
		BrowserModule,
		FontAwesomeModule,
		NgMultiSelectDropDownModule.forRoot(),
		QuillModule.forRoot(),
		RouterModule.forRoot(routes, { useHash: true }),
	],
	providers: [{ provide: LOCALE_ID, useValue: "pe" }, AdminGuard, ClientGuard],
	bootstrap: [AppComponent],
})
export class AppModule {}

import { Component, ElementRef, OnInit, ViewChild } from "@angular/core"
import { Router } from "@angular/router"
import { CleaningService } from "src/app/services/cleaning.service"
import { NotificationService } from "src/app/services/notification.service"
import { ModalComponent } from "../utils/modal/modal.component"

@Component({
	selector: "app-limpieza",
	templateUrl: "./limpieza.component.html",
	styleUrls: ["./limpieza.component.css"],
})
export class LimpiezaComponent implements OnInit {
	form = {
		prefix: { value: "51", invalid: false },
		number: { value: "", invalid: false },
	}
	orderInfo: any = null
	@ViewChild("mPrefix") mPrefix: ElementRef
	@ViewChild("mNumber") mNumber: ElementRef
	@ViewChild("mConfirm") mConfirm: ModalComponent

	constructor(private cleaning: CleaningService, private router: Router, private notif: NotificationService) {
		if (localStorage.number != null) this.form.number.value = localStorage.number
		if (localStorage.prefix != null) this.form.prefix.value = localStorage.prefix
	}

	async ngOnInit(): Promise<void> {
		this.orderInfo = await this.cleaning.getOrderInfo()
	}

	validatePrefix() {
		this.form.prefix.invalid = !/^[0-9]{1,4}$/.test(this.form.prefix.value)
	}
	validateNumber() {
		this.form.number.invalid = !/^[0-9]{9}$/.test(this.form.number.value)
	}
	validateForm(): boolean {
		this.validatePrefix()
		this.validateNumber()

		if (this.form.prefix.invalid) {
			this.mPrefix.nativeElement.focus()
			return false
		}
		if (this.form.number.invalid) {
			this.mNumber.nativeElement.focus()
			return false
		}
		return true
	}

	openModal() {
		if (!this.validateForm()) return
		this.mConfirm.open()
	}

	async save() {
		if (!this.validateForm()) return
		await this.cleaning.saveOrder(
			{
				prefix: this.form.prefix.value,
				number: this.form.number.value,
				price: this.orderInfo.price,
			},
			() => {
				this.router.navigate(["/master/main"])
			}
		)
	}
}

import { Component, ElementRef, OnInit, ViewChild } from "@angular/core"
import { Router } from "@angular/router"
import { InfoService } from "src/app/services/info.service"
import { LateCheckService } from "src/app/services/late-check.service"
import { NotificationService } from "src/app/services/notification.service"
import { ModalComponent } from "../utils/modal/modal.component"

@Component({
	selector: "app-late-checkout",
	templateUrl: "./late-checkout.component.html",
	styleUrls: ["./late-checkout.component.css"],
})
export class LateCheckoutComponent implements OnInit {
	form = {
		prefix: { value: "51", invalid: false },
		number: { value: "", invalid: false },
	}
	@ViewChild("mPrefix") mPrefix: ElementRef
	@ViewChild("mNumber") mNumber: ElementRef
	@ViewChild("mConfirm") mConfirm: ModalComponent

	latecheckoutlist = []
	selected = null

	constructor(private late: LateCheckService, private router: Router, private notif: NotificationService, private info: InfoService) {
		if (localStorage.number != null) this.form.number.value = localStorage.number
		if (localStorage.prefix != null) this.form.prefix.value = localStorage.prefix
	}

	async ngOnInit(): Promise<void> {
		this.latecheckoutlist = await this.info.loadLateCheckout()
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

	showConfirm(index: number) {
		if (!this.validateForm()) return

		this.selected = this.latecheckoutlist[index]
		let now = new Date()
		if (now.getHours() >= this.selected.hourLimit) {
			this.notif.error(`Esta solicitud debe hacerla el mismo día antes de las ${this.selected.hourLimit}:00`)
			return
		}
		this.mConfirm.open()
	}

	async lateCheck() {
		if (!this.validateForm()) return
		await this.late.saveOrder(this.form.prefix.value, this.form.number.value, this.selected, () => {
			this.router.navigate(["/master/main"])
		})
	}
}
